--Егор, вот создание таблицы---

CREATE DATABASE Library;
GO
USE Library;
GO

-- Таблица Книг
CREATE TABLE Books (
    BookId INT PRIMARY KEY IDENTITY,
    Title NVARCHAR(255) NOT NULL,
    Author NVARCHAR(255) NOT NULL,
    PublishYear INT,
    Price DECIMAL(10,2),
    Annotation NVARCHAR(MAX),
    Available INT
);

-- Таблица Экземпляров книги
CREATE TABLE BookCopies (
    CopyId INT PRIMARY KEY IDENTITY,
    BookId INT NOT NULL,
    IsAvailable BIT DEFAULT 1, -- 1 = на полке, 0 = выдана
    FOREIGN KEY (BookId) REFERENCES Books(BookId)
);

-- Таблица Читателей
CREATE TABLE Readers (
    ReaderId INT PRIMARY KEY IDENTITY,
    FullName NVARCHAR(255) NOT NULL,
    Address NVARCHAR(255),
    Phone NVARCHAR(50),
    TicketNumber NVARCHAR(50) UNIQUE NOT NULL
);

-- Таблица Выдачи
CREATE TABLE Loans (
    LoanId INT PRIMARY KEY IDENTITY,
    CopyId INT NOT NULL,
    ReaderId INT NOT NULL,
    LoanDate DATE NOT NULL,
    ReturnDate DATE NULL,
    DueDate DATE NOT NULL,  -- дата, когда нужно вернуть
    IsReturned BIT DEFAULT 0,
    FOREIGN KEY (CopyId) REFERENCES BookCopies(CopyId),
    FOREIGN KEY (ReaderId) REFERENCES Readers(ReaderId)
);


-- Книги
INSERT INTO Books (Title, Author, PublishYear, Price, Annotation)
VALUES 
(N'Война и мир', N'Лев Толстой', 1869, 500.00, N'Роман-эпопея'),
(N'Преступление и наказание', N'Фёдор Достоевский', 1866, 400.00, N'Роман'),
(N'Мастер и Маргарита', N'Михаил Булгаков', 1966, 350.00, N'Роман'),
(N'Гарри Поттер и философский камень', N'Джоан Роулинг', 1997, 600.00, N'Фэнтези'),
(N'Три мушкетёра', N'Александр Дюма', 1844, 300.00, N'Приключения');

-- Экземпляры книг
INSERT INTO BookCopies (BookId, IsAvailable)
VALUES
(1, 1),(1, 1),(1, 1),(1, 1),(1, 1),  -- Война и мир (5 экз.)
(2, 1),(2, 1),(2, 1),                -- Преступление и наказание (3 экз.)
(3, 1),(3, 1),                       -- Мастер и Маргарита (2 экз.)
(4, 1),(4, 1),(4, 1),(4, 1),         -- Гарри Поттер (4 экз.)
(5, 1),(5, 1);                       -- Три мушкетёра (2 экз.)

-- Читатели
INSERT INTO Readers (FullName, Address, Phone, TicketNumber)
VALUES
(N'Иванов Иван Иванович', N'Москва, ул. Ленина, д. 10', N'89000000001', N'TK001'),
(N'Петров Пётр Петрович', N'Санкт-Петербург, Невский пр., д. 20', N'89000000002', N'TK002'),
(N'Сидорова Анна Сергеевна', N'Казань, ул. Баумана, д. 5', N'89000000003', N'TK003'),
(N'Караваев Николай Николаевич', N'Екатеринбург, ул. Мира, д. 15', N'89000000004', N'TK004');

-- Выдачи
INSERT INTO Loans (CopyId, ReaderId, LoanDate, DueDate, ReturnDate, IsReturned)
VALUES
-- Иванов брал "Войну и мир", вернул
(1, 1, '2023-01-15', '2023-02-15', '2023-02-10', 1),

-- Петров взял "Преступление и наказание", просрочил (более 4 мес.)
(6, 2, '2023-01-01', '2023-02-01', NULL, 0),

-- Сидорова читает "Мастер и Маргарита"
(9, 3, '2023-04-10', '2023-05-10', '2023-05-05', 1),

-- Караваев взял "Гарри Поттер" весной 2023, ещё не вернул
(12, 4, '2023-04-15', '2023-05-15', NULL, 0),

-- Караваев взял ещё "Три мушкетёра", не вернул
(15, 4, '2023-04-20', '2023-05-20', NULL, 0);
