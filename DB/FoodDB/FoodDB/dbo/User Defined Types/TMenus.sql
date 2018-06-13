CREATE TYPE [dbo].[TMenus] AS TABLE (
    [id]             INT            NOT NULL,
    [name]           NVARCHAR (256) NULL,
    [meal_period_id] INT            DEFAULT ((1)) NOT NULL,
    [service_date]   DATETIME       DEFAULT (getdate()) NOT NULL);

