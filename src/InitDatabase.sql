-- Departments
CREATE TABLE IF NOT EXISTS deps
(
    id     serial       PRIMARY KEY,
    name   VARCHAR(200) NOT NULL,
    active BOOLEAN      NOT NULL
);

-- Employees
CREATE TABLE IF NOT EXISTS emps
(
    id     serial       PRIMARY KEY,
    name   VARCHAR(200) NOT NULL,
    inn  VARCHAR(12) NOT NULL UNIQUE,
    deps_id INTEGER REFERENCES deps(id)
);

-- Adds 10 departments
DO $$
BEGIN
    FOR i IN 1..10 LOOP
        INSERT INTO deps (name, active) VALUES ('Department ' || i, TRUE);
    END LOOP;
END $$;

-- Insert 1 inactive department
INSERT INTO deps (name, active) VALUES ('Inactive Department', FALSE);

-- Adds 100000 employees across 11 departments
DO $$
BEGIN
    FOR i IN 1..100000 LOOP
        INSERT INTO emps (name, inn, deps_id) VALUES ('Employee ' || i, lpad(i::text, 12, '0'), ((i-1) % 11) + 1);
    END LOOP;
END $$;
