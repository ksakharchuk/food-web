CREATE TABLE [dbo].[categories] (
    [id]                 INT            IDENTITY (1, 1) NOT NULL,
    [name]               NVARCHAR (256) NULL,
    [parent_category_id] INT            NULL,
    [created_date]       DATETIME       DEFAULT (getdate()) NOT NULL,
    [last_update_date]   DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_categories] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_categories_categories] FOREIGN KEY ([parent_category_id]) REFERENCES [dbo].[categories] ([id])
);

