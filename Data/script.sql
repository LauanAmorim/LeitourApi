Drop database if exists dbleitour;
CREATE DATABASE dbleitour;
use dbleitour;

-- Tabela tbUsuario
CREATE TABLE tbUsuario (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nome VARCHAR(30) not null,
    Email VARCHAR(100) not null unique,
    Senha VARCHAR(64) not null,
    Acesso ENUM('Admin', 'Comum','Premium','Desativado') not null default 'comum',
    DataCadastro DATETIME default current_timestamp,
    FotoPerfil varchar(256)
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


-- Tabela tbPublicacoes

CREATE TABLE tbPublicacoes (
    IdPublicacao INT AUTO_INCREMENT PRIMARY KEY,
    IdUsuario INT not null,
    Conteudo VARCHAR(250),
	likes int not null default 0,
    DataCriacao DATETIME default current_timestamp,
    DataAlteracao date,
    FOREIGN KEY (idUsuario) REFERENCES tbUsuario(Id)
);

-- Tabela tbComentarios
CREATE TABLE tbComentarios (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    IdUsuario INT not null,
    IdPublicacao INT not null,
    ConteudoComentario VARCHAR(250),
    DataCriacao DATETIME default current_timestamp,
    DataAlteracao date,
    FOREIGN KEY (IdUsuario) REFERENCES tbUsuario(Id),
    FOREIGN KEY (IdPublicacao) REFERENCES tbPublicacoes(IdPublicacao)
);


-- Tabela Books
create table tbBook(
    Id int primary key auto_increment,
    IdUsuario int not null,
    foreign key(IdUsuario) references tbUsuario(Id),
    BookKey varchar(25) not null,
    Publico tinyint not null default 0
);


-- Tabela tbAnnotation

CREATE TABLE tbAnnotation (
    IddAnnotation INT AUTO_INCREMENT PRIMARY KEY,
    IdUsuario INT not null,
    IdBook INT not null,
    ConteudoAnotacao VARCHAR(250),
    DataAnotacao DATETIME default current_timestamp,
    DataAlteracao date,
    Publico tinyint not null default 0,
    FOREIGN KEY (IdUsuario) REFERENCES tbUsuario(Id),
    FOREIGN KEY (IdBook) REFERENCES tbBook(Id)
);

-- tabela seguidores

create table tbFollowingList(
    IdUsuario int not null,
    foreign key(IdUsuario) references tbUsuario(Id),
    Email varchar(50) not null,
    foreign key(Email) references tbUsuario(Email)
);

insert into tbUsuario(Nome,senha,Email) values('Lucas','12345','Lucas@gmail.com');
insert into tbUsuario(Nome,senha,Email) values('Daniel','12345','Daniel@gmail.com');
insert into tbUsuario(Nome,senha,Email) values('Luiz','12345','Luiz@gmail.com');
