--create database web_blog
--use web_blog

create table BlogUser
(
    Id          nvarchar(255) not null primary key,
    BlogUserId  int           not null identity unique,
    FullName    nvarchar(255) not null,
    Avatar      nvarchar(255) null,
    ModifiedLog varchar(max)  null,
);

create table Article
(
    Id                 int           not null identity primary key,
    Title              nvarchar(255) not null,
    Thumbnail          varchar(255)  null,
    ShortDescription   varchar(max)  null,
    Content            varchar(max)  not null,
    CreateDate         date          null,
    ModifiedDate       date          null,
    ModifiedLog        varchar(max)  null,
    CreateByBlogUserId int           not null,
    constraint FK_user_create_article foreign key (CreateByBlogUserId) references BlogUser (BlogUserId)
);

create table Category
(
    Id   int identity primary key,
    Name varchar(255) not null,
    Code varchar(255) not null
);

create table ArticleCategory
(
    ArticleId  int not null,
    CategoryId int not null,
    constraint FK_article_category_article foreign key (ArticleId) references Article (id),
    constraint FK_article_category_category foreign key (CategoryId) references Category (id)
);

create table Comments
(
    Id                 bigint identity primary key,
    Content            varchar(max) not null,
    CreateDate         date         null,
    ModifiedDate       date         null,
    ModifiedLog        varchar(max) null,
    CreateByBlogUserId int          not null,
    ArticleId          int          not null,
    constraint FK_comments_user foreign key (CreateByBlogUserId) references BlogUser (BlogUserId),
    constraint FK_comments_article foreign key (ArticleId) references Article (id)
);
