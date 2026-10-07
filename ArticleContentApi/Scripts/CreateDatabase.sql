CREATE DATABASE ArticleContentDb;
GO

USE ArticleContentDb;
GO

-- Create Users table
CREATE TABLE Users
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username VARCHAR(200) NOT NULL,
    CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_Users_CreatedAt
        DEFAULT GETUTCDATE()
);
GO

-- Create Articles table
CREATE TABLE Articles
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Status VARCHAR(30) NOT NULL
        CONSTRAINT CK_Articles_Status
        CHECK (Status IN ('Draft', 'Published', 'Unpublished')),
    CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_Articles_CreatedAt
        DEFAULT GETUTCDATE()
);
GO

-- Create Contents table
CREATE TABLE Contents
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title VARCHAR(500) NULL,
    Content VARCHAR(MAX) NOT NULL,
    AuthorId INT NOT NULL,
    Status VARCHAR(30) NOT NULL
        CONSTRAINT CK_Contents_Status
        CHECK (Status IN ('Draft', 'Published', 'Unpublished')),
    CreatedAt DATETIME NOT NULL
        CONSTRAINT DF_Contents_CreatedAt
        DEFAULT GETUTCDATE(),
    Language VARCHAR(30) NOT NULL
        CONSTRAINT CK_Contents_Language
        CHECK (Language IN ('English', 'Marathi', 'Hindi', 'French', 'Spanish')),

    ArticleId INT NOT NULL,
    CONSTRAINT FK_Contents_Users
        FOREIGN KEY (AuthorId)
        REFERENCES Users(Id),

    CONSTRAINT FK_Contents_Articles
        FOREIGN KEY (ArticleId)
        REFERENCES Articles(Id)
        ON DELETE CASCADE
);
GO

-- Indexes
CREATE INDEX IX_Contents_ArticleId_Language
ON Contents(ArticleId, Language);

CREATE INDEX IX_Contents_AuthorId
ON Contents(AuthorId);

CREATE INDEX IX_Articles_CreatedAt
ON Articles(CreatedAt);
GO

-- Create users
INSERT INTO Users (Username)
VALUES
    ('Jack'),
    ('Boby'),
    ('Ram');

-- Create articles
INSERT INTO Articles (Status)
VALUES
    ('Published'),
    ('Draft'),
    ('Unpublished');

-- Create content
INSERT INTO Contents
    (Title, Content, AuthorId, Status, Language, ArticleId)
VALUES
    ('First Article', 'English article content.', 1, 'Published', 'English', 1),
    ('Premier Article', 'Hindi article content.', 2, 'Published', 'Hindi', 1),
    ('Second Article', 'Second article content.', 3, 'Draft', 'English', 2),
    ('Segundo Artículo', 'Spanish article content.', 1, 'Published', 'Spanish', 3);
GO
