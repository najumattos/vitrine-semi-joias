USE DB_Vitrine_Semi_Joias;
GO

INSERT INTO Products (Title, Description, Price, ImageUrl, CategoryEnum, IsAvailable, JewelryCode)
VALUES 
('Anel Solitario de Prata Zirconia', 'Anel solitario classico confeccionado em prata 925 ...', 129.90, 'img/products/0dbbbb54-45e6-4330-9347-59178c45b7de.jpg', 'Anel', 1, 1001),
('Pulseira Com Pingente de Cora��o', 'Pulseira dourada com pingente fixo', 54.00, 'img/products/c03a9129-3562-43f2-b82d-355fa0da8fdd.jpg', 'Pulseira', 1, 1017),
('Pulseira Dupla com Pingentes', 'Pulseira Dupla Dourada com Pingentes', 60.00, 'img/products/e39950b4-e3ad-445b-a0fd-2df55d9b9b64.jpg', 'Pulseira', 1, 1032),
('Pulseira Transada com Pingente', 'Pulseira Transada com Pingente de La�o | Dispon�...', 55.00, 'img/products/c5ca8225-5ba1-45b7-83ea-fe36506e8168.jpg', 'Pulseira', 1, 1033),
('Pulseira Com Pedra Gota', 'Pulseira disponivel com Pedras nas cores Verde, Tr...', 50.00, 'img/products/88e81a35-7747-4ce1-a523-c4cd769bffff.jpg', 'Pulseira', 1, 1035),
('Conjunto Brincos + Colar', 'Disponivel no Prata nas cores Preto, Azul e Verde', 70.00, 'img/products/4371c0f3-961c-4c90-9055-00a4b0b18da8.jpg', 'Kit', 1, 1038),
('Pingente Cachorrinho', 'Disponivel no dourado e no prata', 35.00, 'img/products/60807d53-772f-4655-a009-ba580ad97725.jpg', 'Pingente', 1, 1041),
('Colar F�', NULL, 45.00, 'img/products/f901e1d5-1b52-4e4f-97bb-cd0c5b03d494.jpg', 'Colar', 1, 1046),
('Colar com Pingente Borboleta', 'Disponivel no Dourado', 45.00, 'img/products/b40d468c-b330-4a1b-8e8c-566a0612614e.jpg', 'Colar', 1, 1047),
('Colar Com Brincos prata KIT ', 'Disponivel no Preto, vermelho e azul', 55.00, 'img/products/79dccdda-70e1-41b8-b880-e78ebb722f27.jpg', 'Kit', 1, 10478),
('Trio de Brincos Triangulo', 'Disponivel nas cores Preto, verde, rosa e azul ', 40.00, 'img/products/591b5e66-c233-4d41-b4b9-6191d989492a.jpg', 'Brinco', 1, 1052),
('Brinco no Prata', NULL, 36.00, 'img/products/373a665c-6657-412d-8031-e78ee4754ae6.jpg', 'Brinco', 1, 1053),
('Brinco Dourado', 'Brinco Dourado', 65.00, 'img/products/68d61122-e99d-4b88-a413-160351ea606a.jpg', 'Brinco', 1, 1454),
('Primeira pe�a com descri��o gerada...', 'Aqui est� uma descri��o comercial curta e objetiva:...', 100.00, 'img/products/Jewelry14', 'Brinco', 0, 1515),
('Teste Prompt de Fallback', '**Bolo de Milho Verde com Goiabada** Liquidifica...', 100.00, 'img/products/Jewelry15', 'Brinco', 0, 1115),
('aaaaaaaaaaaaaaaaaaaa', 'aaaaaaaaaaaaaaaaaaaaaaaaaaaaaa', 6660.00, 'img/products/Jewelry16', 'Brinco', 0, 7646);
GO

USE DB_Vitrine_Semi_Joias;
GO
select * from Products;
