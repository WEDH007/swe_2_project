CREATE TABLE public.users (
    id SERIAL PRIMARY KEY,
    first_name character varying(255) NOT NULL,
    last_name character varying(255) NOT NULL,
    password_hash character varying(512) NOT NULL,
    email character varying(255) NOT NULL,
    dob date NOT NULL,
    verification_token character varying(255),
    verified_at timestamp without time zone,
    status text DEFAULT 'inactive'::text NOT NULL
);

