--DROP TABLE users cascade
--select * from tiktok_profiles
--select * From users
CREATE TABLE IF NOT EXISTS public.users
(
    id uuid NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    email varchar NOT NULL,
    password varchar NOT NULL,
	salt varchar not null,
    created_at timestamp NOT NULL DEFAULT now(),
    updated_at timestamp NOT NULL DEFAULT now(),
	profile uuid,
    CONSTRAINT "UQ_usersemail" UNIQUE (email),
	    CONSTRAINT "FK_usersxprofiles" FOREIGN KEY (profile)
        REFERENCES public.profiles (id)
)
