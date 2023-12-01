Drop database if exists db_leitour;
CREATE DATABASE db_leitour;
use db_leitour;

-- Tabela usuario
CREATE TABLE tbl_usuario (
    pk_usuario_id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_nome VARCHAR(30) not null,
    usuario_email VARCHAR(100) not null unique,
    usuario_senha VARCHAR(64) not null,
    usuario_acesso ENUM('Admin', 'Comum','Premium','Desativado') not null default 'Comum',
    data_criacao DATETIME default current_timestamp,
    usuario_foto_perfil varchar(256)
);

-- Tabela publicacao
CREATE TABLE tbl_publicacao (
    pk_publicacao_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_usuario_id INT not null,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id),
    publicacao_texto VARCHAR(250) not null,
    data_criacao DATETIME not null default current_timestamp,
    data_alteracao date
);

-- Tabela comentario
CREATE TABLE tbl_comentario (
    pk_comentario_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_usuario_id INT not null,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id),
    fk_publicacao_id INT not null,
    FOREIGN KEY (fk_publicacao_id) REFERENCES tbl_publicacao(pk_publicacao_id),
    comentario_texto VARCHAR(250) not null,
    data_criacao DATETIME not null default current_timestamp,
    data_alteracao date
);

-- Tabela livros
create table tbl_livro_salvo(
    pk_livro_salvo_id int primary key auto_increment,
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    livro_salvo_chave_livro varchar(25) not null,
    livro_salvo_publico tinyint not null default 0,
    livro_salvo_capa varchar(120) not null,
    livro_salvo_titulo varchar(255) not null

);

-- Tabela anotacao
CREATE TABLE tbl_anotacao (
    pk_anotacao_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_livro_salvo_id INT not null,
    anotacao_texto VARCHAR(250) not null,
    data_criacao DATETIME default current_timestamp,
    data_alteracao date,
    FOREIGN KEY (fk_livro_salvo_id) REFERENCES tbl_livro_salvo(pk_livro_salvo_id)
);

-- tabela seguidores
create table tbl_seguidor(
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    fk_usuario_email varchar(100) not null,
    foreign key(fk_usuario_email) references tbl_usuario(usuario_email),
    primary key(fk_usuario_id,fk_usuario_email)
);

-- tabela curtidas
create table tbl_like(
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    fk_publicacao_id int not null,
    foreign key(fk_publicacao_id) references tbl_publicacao(pk_publicacao_id),
    primary key(fk_usuario_id,fk_publicacao_id)
);


delimiter $$
Create procedure sp_seguidor_seguir(in vIdUser int,in vEmailSeguidor varchar(100))
begin
	insert into tbl_seguidor(fk_usuario_id,fk_usuario_email) values (vIdUser,vEmailSeguidor);
end $$
delimiter ;

delimiter $$
Create procedure sp_select_seguidor_seguintes(in vEmailSeguidor varchar(100))
begin
	select * from tbl_usuario inner JOIN tbl_seguidor on fk_usuario_email = vEmailSeguidor;
end $$
delimiter ;

delimiter $$
Create procedure sp_select_seguidor_seguidores(in vIdUser int)
begin
	select * from tbl_usuario inner JOIN tbl_seguidor on fk_usuario_id = vIdUser;
end $$
delimiter ;

delimiter $$
Create procedure sp_seguidor_desseguir(in vIdUser int,in vEmailSeguidor varchar(100))
begin
	delete from tbl_seguidor where fk_usuario_id = vIdUser and fk_usuario_email = vEmailSeguidor;
end $$
delimiter ;

delimiter $$
Create procedure sp_like(in vIdUser int, in vIdPublicacao int, out vSucesso int)
sp_verificar_like: begin
	set vSucesso := -1;
	SET @usuario := (select pk_usuario_id from tbl_usuario where pk_usuario_id = vIdUser);
    if(@usuario is null)then
		select @vSucesso;
		leave sp_verificar_like;
    end if;
	SET @publicacao := (select pk_publicacao_id from tbl_publicacao where pk_publicacao_id = vIdPublicacao);
    if(@publicacao is null)then
		select @vSucesso;
		leave sp_verificar_like;
    end if;
    set @usuariolikes := (select count(*) from tbl_like where fk_usuario_id = vIdUser and fk_publicacao_id = vIdPublicacao);
	if(@usuariolikes = 0)then
		insert into tbl_like(fk_usuario_id,fk_publicacao_id) values (vIdUser,vIdPublicacao);
        set vSucesso = 1;
	else
		delete from tbl_like where fk_usuario_id = vIdUser and fk_publicacao_id = vIdPublicacao;
        set vSucesso = 0;
	end if;
    select @vSucesso;
end $$
delimiter ;

-- Deletar comentários juntos com a publicacao
DELIMITER $$
CREATE TRIGGER tr_publicacao_before_delete
    BEFORE DELETE
    ON tbl_publicacao FOR EACH ROW
BEGIN
	delete from tbl_comentario where tbl_comentario.fk_publicacao_id = OLD.pk_publicacao_id;
END$$    
DELIMITER ;

-- Apaga as anotações quando se dessalva o livro
DELIMITER $$
CREATE TRIGGER tr_livro_salvo_before_delete
    BEFORE DELETE
    ON tbl_livro_salvo FOR EACH ROW
BEGIN
	delete from tbl_anotacao where tbl_anotacao.fk_livro_salvo_id = OLD.pk_livro_salvo_id;
END$$    
DELIMITER ;

 -- selecionar a publicação e o nome do usuario
 delimiter //
create view vw_comentario as
SELECT tbl_comentario.*,tbl_usuario.usuario_nome,tbl_usuario.usuario_foto_perfil as "usuario_foto"
    FROM tbl_comentario
	Inner JOIN tbl_usuario on pk_usuario_id
    where tbl_usuario.pk_usuario_id = tbl_comentario.fk_usuario_id;
//
delimiter ;
    
delimiter //
create view vw_publicacao as
SELECT tbl_publicacao.*,tbl_usuario.usuario_nome,tbl_usuario.usuario_foto_perfil as "usuario_foto",
	(SELECT COUNT(*) FROM tbl_comentario
        WHERE fk_publicacao_id = pk_publicacao_id) as num_comentario,
    (SELECT COUNT(*) FROM tbl_like
        WHERE fk_publicacao_id = pk_publicacao_id) as likes, (select false) as "liked"
    FROM tbl_publicacao
    Inner JOIN tbl_usuario on pk_usuario_id
    where tbl_usuario.pk_usuario_id = tbl_publicacao.fk_usuario_id;
//
delimiter ;

delimiter //
create procedure sp_select_publicacao(in vIdUsuario int, in vLimite int, in vOffset int)
begin
SELECT tbl_publicacao.*,tbl_usuario.usuario_nome,tbl_usuario.usuario_foto_perfil as "usuario_foto",
	(SELECT COUNT(*) FROM tbl_comentario
        WHERE fk_publicacao_id = pk_publicacao_id) as num_comentario,
    (SELECT COUNT(*) FROM tbl_like
        WHERE fk_publicacao_id = pk_publicacao_id) as likes,
        (select count(*) from tbl_like where fk_usuario_id = pk_usuario_id and fk_publicacao_id = vIdUsuario) as "liked"
    FROM tbl_publicacao
    Inner JOIN tbl_usuario on pk_usuario_id
    where tbl_usuario.pk_usuario_id = tbl_publicacao.fk_usuario_id order by data_criacao desc limit vLimite offset vOffset;
end //
delimiter ;


delimiter //
create view vw_usuario as
SELECT pk_usuario_id,usuario_nome,usuario_email,"" as usuario_senha,"" as usuario_acesso,data_criacao,usuario_foto_perfil FROM tbl_usuario;
//
delimiter ;

delimiter $$
Create procedure sp_liked(vIdUser int)
begin select pk_publicacao_id from tbl_publicacao where tbl_publicacao.pk_publicacao_id = (select fk_publicacao_id from tbl_like where fk_usuario_id = vIdUser);
end $$ delimiter ;

delimiter $$
Create procedure sp_usuario(vIdUser int)
begin
	select * from tbl_usuario where pk_usuario_id = vIdUser;
end $$
delimiter ;

create function usuarioId() returns INTEGER DETERMINISTIC NO SQL return @p1;

create view vw_usuario_estatisticas as
SELECT (select count(*) from tbl_livro_salvo where fk_usuario_id = usuarioId()) as 'Livros salvos',
	(select count(*) from tbl_publicacao where fk_usuario_id = usuarioId()) as 'Posts',
    (select count(*) from tbl_seguidor where fk_usuario_id = usuarioId()) as 'Seguindo',
    (select count(*) from tbl_seguidor where fk_usuario_email = (select usuario_email from tbl_usuario where pk_usuario_id = usuarioId())) as 'Seguidores';

