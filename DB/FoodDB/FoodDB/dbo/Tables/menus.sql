CREATE TABLE [dbo].[menus] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (256) NULL,
    [meal_period_id]   INT            NOT NULL,
    [service_date]     DATETIME       NULL,
    [created_date]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [last_update_date] DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_menus] PRIMARY KEY CLUSTERED ([id] ASC),
    CONSTRAINT [FK_menu_meal_periods] FOREIGN KEY ([meal_period_id]) REFERENCES [dbo].[meal_periods] ([id])
);

