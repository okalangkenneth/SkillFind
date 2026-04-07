-- Create all three databases in a single PostgreSQL instance
CREATE DATABASE skillfind_jobcategory;
CREATE DATABASE skillfind_jobseeker;

-- Grant all privileges to the shared user
GRANT ALL PRIVILEGES ON DATABASE skillfind_jobposting TO skillfind_user;
GRANT ALL PRIVILEGES ON DATABASE skillfind_jobcategory TO skillfind_user;
GRANT ALL PRIVILEGES ON DATABASE skillfind_jobseeker TO skillfind_user;
