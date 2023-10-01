Drop database if exists db_leitour;
CREATE DATABASE db_leitour;
use db_leitour;

-- Tabela tbl_usuario
CREATE TABLE tbl_usuario (
    pk_usuario_id INT AUTO_INCREMENT PRIMARY KEY,
    usuario_nome VARCHAR(30) not null,
    usuario_email VARCHAR(100) not null unique,
    usuario_senha VARCHAR(64) not null,
    usuario_acesso ENUM('Admin', 'Comum','Premium','Desativado') not null default 'comum',
    usuario_data_cadastro DATETIME default current_timestamp,
    usuario_foto_perfil varchar(256)
);

-- Tabela tbLivros
CREATE TABLE tbGenero(
	generoId int primary key,
    genero varchar(250)
);

/* - Tabela Para internal storage
CREATE TABLE tbLivros (
    ISBN INT PRIMARY KEY,
    Capa varchar(256),
    Titulo VARCHAR(250) not null,
    Autor VARCHAR(250) not null,
    Sinopse varchar(250),
    Paginas int not null,
    GeneroID INT NOT NULL,
    DataPublicacao DATETIME,
    Avaliacao FLOAT,
    FOREIGN KEY (GeneroID) REFERENCES tbGenero(Id)
);*/


-- Tabela tbl_publicacao

CREATE TABLE tbl_publicacao (
    pk_publicacao_id INT AUTO_INCREMENT PRIMARY KEY,
    fk_usuario_id INT not null,
    FOREIGN KEY (fk_usuario_id) REFERENCES tbl_usuario(pk_usuario_id),
    publicacao_texto VARCHAR(250) not null,
	publicacao_like int not null default 0,
    publicacao_data_criacao DATETIME default current_timestamp,
    publicacao_data_alteracao date
);

-- Tabela tbl_comentario
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


-- Tabela Books
create table tbl_livro_salvo(
    pk_livro_salvo_id int primary key auto_increment,
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    livro_salvo_chave_livro varchar(25) not null,
    livro_salvo_publico tinyint not null default 0
);


-- Tabela tbl_anotacao

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
    foreign key(fk_usuario_email) references tbl_usuario(usuario_email)
);

create table tbl_like(
    fk_usuario_id int not null,
    foreign key(fk_usuario_id) references tbl_usuario(pk_usuario_id),
    fk_publicacao_id int not null,
    foreign key(fk_publicacao_id) references tbl_publicacao(pk_publicacao_id)
);


delimiter $$
Create procedure sp_seguidor_seguir(in vIdUser int,in vEmailSeguidor varchar(100))
begin
	insert into tbl_seguidores(fk_usuario_id,fk_usuario_email) values (vIdUser,vEmailSeguidor);
end $$
delimiter ;

delimiter $$
Create procedure sp_seguidor_desseguir(in vIdUser int)
begin
	delete from tbl_seguidores where fk_usuario_id = vIdUser;
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


DELIMITER $$
CREATE TRIGGER tr_publicacao_before_delete
    BEFORE DELETE
    ON tbl_publicacao FOR EACH ROW
BEGIN
	delete from tbl_comentario where tbl_comentario.fk_publicacao_id = OLD.pk_publicacao_id;
END$$    
DELIMITER ;

DELIMITER $$
CREATE TRIGGER tr_publicacao_after_update
    BEFORE DELETE
    ON tbl_publicacao FOR EACH ROW
BEGIN
	update tbl_publicacao set publicacao_data_alteracao = current_timestamp
    where tbl_publicacao.pk_publicacao_id = OLD.pk_publicacao_id;
END$$    
DELIMITER ;

DELIMITER $$
CREATE TRIGGER tr_comentario_after_update
    BEFORE DELETE
    ON tbl_comentario FOR EACH ROW
BEGIN
	update tbl_comentario set comentario_data_alteracao = current_timestamp
    where tbl_comentario.pk_comentario_id = OLD.pk_comentario_id;
END$$    
DELIMITER ;

DELIMITER $$
CREATE TRIGGER tr_anotacao_after_update
    BEFORE DELETE
    ON tbl_anotacao FOR EACH ROW
BEGIN
	update tbl_anotacao set anotacao_data_alteracao = current_timestamp
    where tbl_anotacao.pk_anotacao_id = OLD.pk_anotacao_id;
END$$    
DELIMITER ;

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
