CREATE TABLE [dbo].[menu_items] (
    [id]               INT             IDENTITY (1, 1) NOT NULL,
    [menu_id]          INT             NOT NULL,
    [ingredient_id]    INT             NOT NULL,
    [weight]           DECIMAL (10, 2) NOT NULL,
    [created_date]     DATETIME        DEFAULT (getdate()) NOT NULL,
    [last_update_date] DATETIME        DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_menu_items] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_menu_items_ingredients] FOREIGN KEY ([ingredient_id]) REFERENCES [dbo].[ingredients] ([id]),
    CONSTRAINT [FK_menu_items_menus] FOREIGN KEY ([menu_id]) REFERENCES [dbo].[menus] ([id])
);

