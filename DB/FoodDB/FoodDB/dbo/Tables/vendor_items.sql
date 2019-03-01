CREATE TABLE [dbo].[vendor_items]
(
    [barcode]           CHAR (13)       NOT NULL,
    [name]              NVARCHAR (256)  NOT NULL,
    [brand_id]          INT             NULL,

    CONSTRAINT [PK_vendor_items] PRIMARY KEY CLUSTERED ([barcode] ASC)
)
