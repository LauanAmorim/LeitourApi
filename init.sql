Drop database if exists db_leitour;
CREATE DATABASE db_leitour;
use db_leitour;

-- Tabela usuario
CREATE TABLE tbl_usuario (
    pk_usuario_id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_nome VARCHAR(30) not null,
    usuario_email VARCHAR(100) not null unique,
    usuario_senha VARCHAR(64) not null,
    usuario_acesso ENUM('Admin', 'Comum','Premium','Desativado') not null default 'comum',
    usuario_data_cadastro DATETIME default current_timestamp,
    usuario_foto_perfil varchar(256)
);

-- Tabela publicacao

CREATE TABLE tbl_publicacao (
    pk_publicacao_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_usuario_id INT not null,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id),
    publicacao_texto VARCHAR(250) not null,
    publicacao_data_criacao DATETIME default current_timestamp,
    publicacao_data_alteracao date
);

-- Tabela comentario
CREATE TABLE tbl_comentario (
    pk_comentario_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_usuario_id INT not null,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id),
    fk_publicacao_id INT not null,
    FOREIGN KEY (fk_publicacao_id) REFERENCES tbl_publicacao(pk_publicacao_id),
    comentario_texto VARCHAR(250) not null,
    comentario_data_criacao  DATETIME default current_timestamp,
    comentario_data_alteracao date
);


-- Tabela livros
create table tbl_livro_salvo(
    pk_livro_salvo_id int primary key auto_increment,
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    livro_salvo_chave_livro varchar(25) not null,
    livro_salvo_publico tinyint not null default 0,
    livro_salvo_capa varchar(100) not null,
    livro_salvo_titulo varchar(50) not null

);


-- Tabela anotacao

CREATE TABLE tbl_anotacao (
    pk_anotacao_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_livro_salvo_id INT not null,
    anotacao_texto VARCHAR(250) not null,
    anotacao_data_criacao DATETIME default current_timestamp,
    anotacao_data_alteracao date,
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
Create procedure sp_like(in vIdUser int, in vIdPublicacao int)
begin
	insert into tbl_like(fk_usuario_id,fk_publicacao_id) values (vIdUser,vIdPublicacao);
end $$
delimiter ;

delimiter $$
Create procedure sp_deslike(in vIdUser int, in vIdPublicacao int)
begin
	delete from tbl_like where fk_usuario_id = vIdUser and fk_publicacao_id = vIdPublicacao;
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

-- Adicionar a data da ultima alteração
DELIMITER $$
CREATE TRIGGER tr_publicacao_after_update
    BEFORE update
    ON tbl_publicacao FOR EACH ROW
BEGIN
	update tbl_publicacao set publicacao_data_alteracao = current_timestamp
    where tbl_publicacao.pk_publicacao_id = OLD.pk_publicacao_id;
END$$    
DELIMITER ;

-- Adicionar a data da ultima alteração
DELIMITER $$
CREATE TRIGGER tr_anotacao_after_update
    BEFORE update
    ON tbl_anotacao FOR EACH ROW
BEGIN
	update tbl_anotacao set anotacao_data_alteracao = current_timestamp
    where tbl_anotacao.pk_anotacao_id = OLD.pk_anotacao_id;
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


insert into tbl_usuario(usuario_nome,usuario_senha,usuario_email) values('Lucas','12345','Lucas@gmail.com');
insert into tbl_usuario(usuario_nome,usuario_senha,usuario_email) values('Daniel','12345','Daniel@gmail.com');
insert into tbl_usuario(usuario_nome,usuario_senha,usuario_email) values('Luiz','12345','Luiz@gmail.com');
insert into tbl_usuario(usuario_nome,usuario_email,usuario_senha) values ("jose","jose@email.com","1234"),("maria","maria@email.com","4321");


insert into tbl_publicacao(fk_usuario_id,publicacao_texto) values(1,'Tesytando');
insert into tbl_publicacao(fk_usuario_id,publicacao_texto) values(3,'Tesytando');
insert into tbl_publicacao(fk_usuario_id,publicacao_texto) values(5,'Tesytando');

call sp_seguidor_seguir(1,'Daniel@gmail.com');
select * from tbl_seguidor;
call sp_seguidor_desseguir(1,'Daniel@gmail.com');
select * from tbl_seguidor;


insert into tbl_comentario(fk_usuario_id,fk_publicacao_id,comentario_texto) values(1,1,'Eai man');


 -- selecionar a publicação e o nome do usuario
 delimiter //
create view vw_comentario as
SELECT tbl_comentario.*,tbl_usuario.usuario_nome
    FROM tbl_comentario
	Inner JOIN tbl_usuario on pk_usuario_id
    where tbl_usuario.pk_usuario_id = tbl_comentario.fk_usuario_id;
//
delimiter ;
    
delimiter //
create view vw_publicacao as
SELECT tbl_publicacao.*,tbl_usuario.usuario_nome,
    (SELECT COUNT(*) FROM tbl_like
        WHERE fk_publicacao_id = pk_publicacao_id) as likes
    FROM tbl_publicacao
    Inner JOIN tbl_usuario on pk_usuario_id
    where tbl_usuario.pk_usuario_id = tbl_publicacao.fk_usuario_id;
//
delimiter ;


delimiter //
create view vw_usuario as
SELECT pk_usuario_id,usuario_nome,usuario_email,usuario_data_cadastro,usuario_foto_perfil FROM tbl_usuario;
//
delimiter ;

