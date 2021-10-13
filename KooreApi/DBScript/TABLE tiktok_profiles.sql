--select * from public.tiktok_profiles
-- DROP TABLE public.tiktok_profiles cascade;
CREATE TABLE IF NOT EXISTS public.tiktok_profiles
(
    uid uuid NOT NULL DEFAULT uuid_generate_v4() PRIMARY KEY,
    id_origin bigint,
    username varchar,
    nickname varchar,
    avatar_thumb text,
    avatar_medium text,
    avatar_larger text,
    signature varchar,
    verified boolean,
    sec_uid text,
    bio_link varchar,
    followers bigint,
    following bigint,
    videos bigint,
    hearts bigint,
    diggs bigint,
    created_at timestamp NOT NULL,
    updated_at timestamp NOT NULL,
    user_id uuid,
    CONSTRAINT tiktok_profiles_user_id_fkey FOREIGN KEY (user_id)
        REFERENCES public.users (id) MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE SET NULL
)
