CREATE TYPE [dbo].[TMenuItems] AS TABLE (
    [id]            INT             NOT NULL,
    [menu_id]       INT             NOT NULL,
    [ingredient_id] INT             NOT NULL,
    [weight]        DECIMAL (10, 2) DEFAULT ((0)) NULL);

