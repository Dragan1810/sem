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

* MT

```sql
CREATE TABLE [dbo].[PlaneType] (
    [Id]   INT          IDENTITY (1, 1) NOT NULL,
    [Code] VARCHAR (20) NOT NULL,
    [Name] VARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[Plane] (
    [Id]          INT          IDENTITY (1, 1) NOT NULL,
    [Code]        VARCHAR (20) NOT NULL,
    [Name]        VARCHAR (50) NOT NULL,
    [PlaneTypeId] INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Plane_PlaneTypeId] FOREIGN KEY ([PlaneTypeId]) REFERENCES [dbo].[PlaneType] ([Id])
);

CREATE TABLE [dbo].[Passanger] (
    [Id]        INT          IDENTITY (1, 1) NOT NULL,
    [FirstName] VARCHAR (20) NOT NULL,
    [LastName]  VARCHAR (50) NOT NULL,
    [PlaneId]   INT          NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Passanger_Plane] FOREIGN KEY ([PlaneId]) REFERENCES [dbo].[Plane] ([Id])
);
```
