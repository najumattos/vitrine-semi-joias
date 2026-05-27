CREATE DATABASE Estoque_DB;
GO

USE Estoque_DB;
GO

-----------TABELAS----------------------
CREATE TABLE Produtos(
	id_produto NUMERIC(5) PRIMARY KEY IDENTITY,
	nome NVARCHAR(200) NOT NULL,
	categoria NUMERIC(5) NOT NULL,
);
-------
CREATE TABLE ProdutoCategoria(
	id_categoria NUMERIC(5) PRIMARY KEY IDENTITY,
	categoria NVARCHAR(200) NOT NULL
);
-------
CREATE TABLE ProdutoEstoque(
	id_produto_estoque NUMERIC(5) PRIMARY KEY IDENTITY,
	quantidade NUMERIC(5) NOT NULL,
	produto NUMERIC(5) NOT NULL
);

--------
CREATE TABLE Clientes(
	id_cliente NUMERIC(5) PRIMARY KEY IDENTITY,
	titulo NVARCHAR(4),
	primeiro_nome NVARCHAR(10) NOT NULL,
	segundo_nome NVARCHAR(10),
	sobrenome NVARCHAR(10) NOT NULL
);
GO
-------------FIM-TABELAS--------------------------

-----------FKS------------
ALTER TABLE ProdutoEstoque 
		ADD CONSTRAINT fk_produto_estoque FOREIGN KEY (produto) REFERENCES Produtos (id_produto)
---------
ALTER TABLE Produtos 
		ADD CONSTRAINT fk_produto_categoria FOREIGN KEY (categoria) REFERENCES ProdutoCategoria (id_categoria)
GO
-----------FIM-FKS-------------

------------CLIENTES------------------------------------------------------------------
INSERT INTO Clientes (titulo, primeiro_nome, segundo_nome, sobrenome)
VALUES (NULL, 'Ana', 'Julia', 'Mattos');
--------
INSERT INTO Clientes (titulo, primeiro_nome, segundo_nome, sobrenome)
VALUES ('Sra.', 'Maria', 'Joaquina', 'Medsen');
--------
INSERT INTO Clientes (titulo, primeiro_nome, segundo_nome, sobrenome)
VALUES ('Sr.', 'Ozzy', NULL, 'Osbourne');
GO
-------------FIM-CLIENTES---------------------------------------------------------------------


-------------CATEGORIAS----------------------------------
INSERT INTO ProdutoCategoria (categoria)
VALUES ('Roupas');
INSERT INTO ProdutoCategoria (categoria)
VALUES ('Calçados');
INSERT INTO ProdutoCategoria (categoria)
VALUES ('Enxoval');
GO
-------------FIM-CATEGORIAS----------------------------------



-------------PRODUTOS------------------------------------
INSERT INTO Produtos (nome, categoria)
VALUES ('Calça', 1);
INSERT INTO Produtos (nome, categoria)
VALUES ('Blusa', 1);
INSERT INTO Produtos (nome, categoria)
VALUES ('Vestido', 1);

INSERT INTO Produtos (nome, categoria)
VALUES ('Tênis', 2);
INSERT INTO Produtos (nome, categoria)
VALUES ('Sapatilha', 2);
INSERT INTO Produtos (nome, categoria)
VALUES ('Chinelo', 2);
INSERT INTO Produtos (nome, categoria)
VALUES ('Bota', 2);
INSERT INTO Produtos (nome, categoria)
VALUES ('Rasteirinha', 2);
INSERT INTO Produtos (nome, categoria)
VALUES ('Sandália', 2);

INSERT INTO Produtos (nome, categoria)
VALUES ('Toalha', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Lençol', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Fronha', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Edredom', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Manta', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Travesseiro', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Mosqueteiro', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Naninha', 3);
INSERT INTO Produtos (nome, categoria)
VALUES ('Colchão', 3);
GO
-------------FIM-PRODUTOS------------------------------------


---------------ESTOQUE----------------------------------
INSERT INTO ProdutoEstoque (quantidade, produto)
VALUES (10, 1); --10 calças
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (15, 2); --15 Blusas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (8, 3); --8 Vestidos

INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (27, 4); --27 Tênis
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (3, 5); --3 Sapatilhas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (6, 6); --6 Chinelos
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (4, 7); --4 Botas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (0, 8); --0 Rasteirinhas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (3, 9); --3 Sandálias

INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (5, 10); --5 Toalhas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (9, 11); --9 Lençõis
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (13, 12); --13 Fronhas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (2, 13); --2 Edredons
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (7, 14); --7 Mantas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (1, 15); --1 Travesseiro
INSERT INTO ProdutoEstoque (quantidade, produto)
VALUES (1, 16); --1 Mosqueteiro
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (2, 17); --2 Naninhas
INSERT INTO ProdutoEstoque  (quantidade, produto)
VALUES (0, 18); --0 Colchões
--------------FIM-ESTOQUE-------------------------------

select * from dbo.Produtos
select * from dbo.ProdutoCategoria
select * from dbo.ProdutoEstoque