-- DROP TABLE public.profiles;
-- select * From profiles
CREATE TABLE IF NOT EXISTS public.profiles
(
    id uuid NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    first_name varchar NOT NULL,
    last_name varchar NOT NULL,
    created_at timestamp NOT NULL DEFAULT now(),
    updated_at timestamp NOT NULL DEFAULT now()
)
