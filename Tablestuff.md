```sql
CREATE TABLE [dbo].[City] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Code] VARCHAR (20) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Collage] (
    [Id]     INT          IDENTITY (1, 1) NOT NULL,
    [Code]   VARCHAR (20) NOT NULL,
    [Name]   VARCHAR (50) NOT NULL,
    [CityId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Collage_City] FOREIGN KEY ([CityId]) REFERENCES [dbo].[City] ([Id])
);

CREATE TABLE [dbo].[Course] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [Code]      VARCHAR (20) NOT NULL,
    [Name]      VARCHAR (50) NOT NULL,
    [CollageId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Course_Collage] FOREIGN KEY ([CollageId]) REFERENCES [dbo].[Collage] ([Id])
);
```