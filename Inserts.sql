use DB_Vitrine_Semi_Joias;
INSERT INTO Products 
(
   Title, Description, Price, ImageUrl, StockQuantity, CategoryEnum, IsAvailable, IsLastUnits, JewelryCode)
VALUES 
(  
    'Anel de Prata Solitário',            -- Title (varchar/nvarchar)
    'Anel em prata 950 com zircônia',     -- Description (varchar/nvarchar)
    189.90,                               -- Price (decimal)
    'https:// some-cdn.com/anel.jpg', -- ImageUrl (varchar/nvarchar)
    15,                                   -- StockQuantity (int)
    2,                                    -- CategoryEnum (int - valor numérico do Enum)
    1,                                    -- IsAvailable (bit - 1 para true, 0 para false)
    0,                                     -- IsLastUnits (bit - 1 para true, 0 para false)
	1024                                 -- JewelryCode (int)
);





select * from Products;