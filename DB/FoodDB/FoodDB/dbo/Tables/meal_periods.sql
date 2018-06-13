CREATE TABLE [dbo].[meal_periods] (
    [id]               INT            IDENTITY (1, 1) NOT NULL,
    [name]             NVARCHAR (256) NULL,
    [created_date]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [last_update_date] DATETIME       DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_meal_periods] PRIMARY KEY CLUSTERED ([id] ASC)
);

