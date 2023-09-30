drop database if exists dbleitour;
create database dbleitour;
use dbleitour;

create table tbUser(
	Id int primary key auto_increment,
    NameUser varchar(20) not null,
    Password varchar(20) not null,
    Email varchar(50) not null unique,
    profilePhoto varchar(50) 
);

create table tbPage(
    PageId int primary key auto_increment,
    NamePage varchar(20) not null,
    DescriptionPage varchar(500),
    Cover blob,
    CreatedDate datetime default CURRENT_TIMESTAMP,
    alteratedDate datetime,
    ActivePage tinyint not null default 1
);

create table tbFollowingPage(
    Id int not null,
    PageId int not null,
    primary key(Id,PageId),
    foreign key(PageId) references tbPage(PageId),
    foreign key(Id) references tbUser(Id),
    RoleUser int(3) not null default 0
);

create table tbPost(
	postId int primary key auto_increment,
    Id int not null,
    foreign key(Id) references tbUser(Id),
    PageId int,
    foreign key(PageId) references tbPage(PageId),
    messagePost varchar(280) not null,
    likes int not null default 0,
    media blob,
    postDate datetime default CURRENT_TIMESTAMP,
    alteratedDate datetime
);

create table tbFollowingList(
    Id int not null,
    foreign key(Id) references tbUser(Id),
    FollowingEmail varchar(50) not null,
    foreign key(FollowingEmail) references tbUser(Email)
);

create table tbBookPage(
    BookKey varchar(25) not null,
    PageId int not null,
    primary key(BookKey,PageId),
    foreign key(PageId) references tbPage(PageId)
);

create table tbSavedBook(
    SavedId int primary key auto_increment,
    Id int not null,
    foreign key(Id) references tbUser(Id),
    BookKey varchar(25) not null,
    public tinyint not null default 0
);

create table tbAnnotation(
    AnnotationId int primary key auto_increment,
    SavedId int not null,
    foreign key(SavedId) references tbSavedBook(SavedId),
    AnnotationText varchar(250) not null,
    alteratedDate datetime not null default CURRENT_TIMESTAMP
);

insert into tbUser(NameUser,Password,Email) values('Lucas','12345','Lucas@gmail.com');
insert into tbUser(NameUser,Password,Email) values('Daniel','12345','Daniel@gmail.com');
insert into tbUser(NameUser,Password,Email) values('Luiz','12345','Luiz@gmail.com');

insert into tbPost(Id,messagePost) values (1,'Primeira vez usando a plataforma');
insert into tbPost(Id,messagePost,likes) values (2,'Queria doar alguns livros, alguem tem interesse?',10);

insert into tbFollowingList values (1,'Daniel@gmail.com');
insert into tbFollowingList values (1,'Luiz@gmail.com');
insert into tbFollowingList values (2,'Lucas@gmail.com');

insert into tbPage(NamePage, DescriptionPage) values ('Potter Heads','Uma pagina para a discussão de todos os livros da série harry potter');
insert into tbFollowingPage values(3,1,2);
insert into tbFollowingPage values(1,1,1);
insert into tbFollowingPage values(2,1,0);

insert into tbBookPage values('GjgQCwAAQBAJ',1);
insert into tbBookPage values('qrv6zwEACAAJ',1);
insert into tbBookPage values('qDYQCwAAQBAJ',1);
insert into tbBookPage values('rUIJPwAACAAJ',1);
insert into tbBookPage values('rkr7zwEACAAJ',1);
insert into tbBookPage values('yjUQCwAAQBAJ',1);

insert into tbPost(PageId,Id,messagePost,likes) values (1,2,'Alguem sabe de uma livraria onde vendem todos os livros?',2);