create database web_blog
use web_blog

create table [user]
(
    id           int           not null identity primary key,
    user_name    varchar(255)  not null unique,
    password     varchar(255)  not null,
    phone_number varchar(255)  null,
    full_name    nvarchar(255) not null,
    email        nvarchar(255) null,
    avatar       nvarchar(255) null,
    modified_log varchar(max)  null,
    level        int
);

create table article
(
    id                int           not null identity primary key,
    title             nvarchar(255) not null,
    thumbnail         varchar(255)  null,
    short_description varchar(max)  null,
    content           varchar(max)  not null,
    create_date       date     null,
    modified_date     date     null,
    modified_log      varchar(max)  null,
    create_by_id      int           not null,
    constraint FK_user_create_article foreign key (create_by_id) references [user] (id)
);

create table category
(
    id   int identity primary key,
    name varchar(255) not null,
    code varchar(255) not null
);

create table article_category
(
    article_id  int not null,
    category_id int not null,
    constraint FK_article_category_article foreign key (article_id) references article (id),
    constraint FK_article_category_category foreign key (category_id) references category (id)
);

create table comments
(
    id            bigint identity primary key,
    content       varchar(max) not null,
    create_date   date    null,
    modified_date date    null,
    create_by_id  varchar(255) null,
    modified_log  varchar(max) null,
    user_id       int          not null,
    article_id    int          not null,
    constraint FK_comments_user foreign key (user_id) references [user] (id),
    constraint FK_comments_article foreign key (article_id) references article (id)
);
