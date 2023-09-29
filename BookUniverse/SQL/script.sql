CREATE TABLE IF NOT EXISTS "Category"
(
    id serial PRIMARY KEY,
    category_name character varying(50) NOT NULL
);

CREATE TABLE IF NOT EXISTS "Book"
(
    id serial PRIMARY KEY,
    title character varying(100) NOT NULL,
    author character varying(50) NOT NULL,
    path character varying(255) NOT NULL,
    description character varying(500) NOT NULL,
    number_of_pages integer NOT NULL,
    rating double precision NOT NULL,
    category_id integer NOT NULL,
    CONSTRAINT category_id_fk FOREIGN KEY (category_id)
        REFERENCES "Category" (id)
);

CREATE TABLE IF NOT EXISTS "User"
(
    id serial PRIMARY KEY,
    username character varying(50) UNIQUE NOT NULL,
    email character varying(255) UNIQUE NOT NULL,
    password character varying(255) NOT NULL,
    role integer NOT NULL
);

CREATE TABLE IF NOT EXISTS "Folder"
(
    id serial PRIMARY KEY,
    folder_name character varying(70) NOT NULL,
    user_id integer NOT NULL,
    CONSTRAINT user_id_fk FOREIGN KEY (user_id)
        REFERENCES "User" (id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "UserBook"
(
    id serial PRIMARY KEY,
    user_id integer NOT NULL,
    book_id integer NOT NULL,
    is_favourite boolean NOT NULL DEFAULT false,
    CONSTRAINT book_id_fk FOREIGN KEY (book_id)
        REFERENCES "Book" (id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT user_id_fk FOREIGN KEY (user_id)
        REFERENCES "User" (id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);

CREATE TABLE IF NOT EXISTS "BookFolder"
(
    id serial PRIMARY KEY,
    book_id integer NOT NULL,
    folder_id integer NOT NULL,
    CONSTRAINT book_id_fk FOREIGN KEY (book_id)
        REFERENCES "Book" (id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE,
    CONSTRAINT folder_id_fk FOREIGN KEY (folder_id)
        REFERENCES "Folder" (id)
        ON UPDATE NO ACTION
        ON DELETE CASCADE
);