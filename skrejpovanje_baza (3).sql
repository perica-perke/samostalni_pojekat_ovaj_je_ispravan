create database skrejpovanje_neko
use skrejpovanje

CREATE TABLE websajtovi (
    websajt_id INT IDENTITY(1,1) PRIMARY KEY,
    ime VARCHAR(255) NOT NULL UNIQUE,
    opis TEXT
);
GO

CREATE TABLE skrejpovi (
    skrejp_id INT IDENTITY(1,1) PRIMARY KEY,
    websajt_id INT NOT NULL,
    skrejp_informacija_1 TEXT NOT NULL,
    skrejp_informacija_2 TEXT NOT NULL,
    skrejp_informacija_3 TEXT NOT NULL,
    CONSTRAINT FK_skrejpovi_websajtovi FOREIGN KEY (websajt_id)
        REFERENCES websajtovi(websajt_id)
        ON DELETE CASCADE
);
GO

CREATE TABLE korisnik (
    korisnik_id INT IDENTITY(1,1) PRIMARY KEY,
    ime VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL
);
GO

CREATE TABLE utakmice (
    utakmica_id INT IDENTITY(1,1) PRIMARY KEY,
    korisnik_id INT NOT NULL,
    skrejp_id INT NOT NULL,
    predikcija VARCHAR(255) NOT NULL,
    CONSTRAINT FK_utakmice_korisnik FOREIGN KEY (korisnik_id)
        REFERENCES korisnik(korisnik_id)
        ON DELETE CASCADE,
    CONSTRAINT FK_utakmice_skrejp FOREIGN KEY (skrejp_id)
        REFERENCES skrejpovi(skrejp_id)
        ON DELETE CASCADE
);
GO

CREATE PROCEDURE insert_websajt
    @ime VARCHAR(255),
    @opis TEXT
AS
BEGIN
    INSERT INTO websajtovi (ime, opis)
    VALUES (@ime, @opis);
END;
GO

CREATE PROCEDURE insert_skrejp
    @websajt_id INT,
    @info1 TEXT,
    @info2 TEXT,
    @info3 TEXT
AS
BEGIN
    INSERT INTO skrejpovi (websajt_id, skrejp_informacija_1, skrejp_informacija_2, skrejp_informacija_3)
    VALUES (@websajt_id, @info1, @info2, @info3);
END;
GO

CREATE PROCEDURE insert_korisnik
    @ime VARCHAR(100),
    @password_hash VARCHAR(255)
AS
BEGIN
    INSERT INTO korisnik (ime, password_hash)
    VALUES (@ime, @password_hash);
END;
GO

CREATE PROCEDURE insert_utakmica
    @korisnik_id INT,
    @skrejp_id INT,
    @predikcija VARCHAR(255)
AS
BEGIN
    INSERT INTO utakmice (korisnik_id, skrejp_id, predikcija)
    VALUES (@korisnik_id, @skrejp_id, @predikcija);
END;
GO

CREATE PROCEDURE update_websajt
    @id INT,
    @ime VARCHAR(255),
    @opis TEXT
AS
BEGIN
    UPDATE websajtovi
    SET ime = @ime,
        opis = @opis
    WHERE websajt_id = @id;
END;
GO

CREATE PROCEDURE update_skrejp
    @id INT,
    @websajt_id INT,
    @info1 TEXT,
    @info2 TEXT,
    @info3 TEXT
AS
BEGIN
    UPDATE skrejpovi
    SET websajt_id = @websajt_id,
        skrejp_informacija_1 = @info1,
        skrejp_informacija_2 = @info2,
        skrejp_informacija_3 = @info3
    WHERE skrejp_id = @id;
END;
GO

CREATE PROCEDURE update_korisnik
    @id INT,
    @ime VARCHAR(100),
    @password_hash VARCHAR(255)
AS
BEGIN
    UPDATE korisnik
    SET ime = @ime,
        password_hash = @password_hash
    WHERE korisnik_id = @id;
END;
GO

CREATE PROCEDURE update_utakmica
    @id INT,
    @korisnik_id INT,
    @skrejp_id INT,
    @predikcija VARCHAR(255)
AS
BEGIN
    UPDATE utakmice
    SET korisnik_id = @korisnik_id,
        skrejp_id = @skrejp_id,
        predikcija = @predikcija
    WHERE utakmica_id = @id;
END;
GO

CREATE PROCEDURE delete_websajt
    @id INT
AS
BEGIN
    DELETE FROM websajtovi
    WHERE websajt_id = @id;
END;
GO

CREATE PROCEDURE delete_skrejp
    @id INT
AS
BEGIN
    DELETE FROM skrejpovi
    WHERE skrejp_id = @id;
END;
GO

CREATE PROCEDURE delete_korisnik
    @id INT
AS
BEGIN
    DELETE FROM korisnik
    WHERE korisnik_id = @id;
END;
GO

CREATE PROCEDURE delete_utakmica
    @id INT
AS
BEGIN
    DELETE FROM utakmice
    WHERE utakmica_id = @id;
END;
GO



