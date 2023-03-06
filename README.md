# Skill Find

## An end-to-end job portal solution with a modern microservices architecture and CI/CD pipeline

Table of Contents:
Table of contents
1. [Introduction](#introduction)
    - [Overview](#overview)
2. [Architecture and design](#architecture-and-design)
3. [Technologies Used](#technologies)


### Overview:


Our job portal application is a modern, microservices-based platform designed to connect job seekers with potential employers. Built using the latest technologies and frameworks, our application offers a wide range of features and capabilities to help users find and apply for their dream jobs.

#### Key Features:

- User-friendly interface for job seekers and employers
- Real-time job notifications and alerts
- Advanced job search capabilities with filtering and sorting options
- Job recommendation engine based on machine learning algorithms
- Identity and access management using IdentityServer4
- API gateway using Ocelot
- Message queueing using RabbitMQ and MassTransit
- Scalability using Kubernetes and Docker containerization
- Centralized logging using Elasticsearch, Logstash, and Kibana (ELK stack)
- CI/CD pipeline using GitHub Actions

Our application is designed to be highly scalable and modular, with a microservices architecture that enables seamless integration of new features and services. We have implemented Domain-Driven Design (DDD) and Command-Query Responsibility Segregation (CQRS) patterns to achieve loose coupling between services and maintain high system availability.

### Architecture Overview:

![image](https://user-images.githubusercontent.com/68539411/223212580-1beef704-b842-42f2-baf9-c56c318ec17f.png)



Our job portal application is built using a microservices architecture, which enables us to build and deploy individual services independently, making it easier to maintain and scale our system. Our microservices are designed to be loosely coupled and communicate with each other through APIs using asynchronous messaging. We have implemented the following core services:

#### Job Seeker Service:
The Job Seeker Service handles user authentication and authorization, as well as managing user profiles and preferences. It uses IdentityServer4 for identity management and implements REST APIs for communication with other services.

#### Job Posting Service:
The Job Posting Service manages job postings submitted by employers. It exposes REST APIs for job posting creation, retrieval, and search.

#### Job Category Service:
The Job Category Service manages job categories and implements REST APIs for category creation, retrieval, and search.

#### Notification Service:
The Notification Service sends real-time notifications to job seekers based on their preferences and job availability. It uses RabbitMQ and MassTransit for message queueing and implements REST APIs for communication with other services.

#### API Gateway:
The API Gateway acts as a single entry point for all external requests to our microservices. It is built using Ocelot and implements REST APIs for communication with our microservices.

#### Search Service:
The Search Service provides advanced job search capabilities with filtering and sorting options based on job category, location, experience, and more. It is built using Elasticsearch and implements REST APIs for search functionality.

#### Logging Service:
The Logging Service is responsible for collecting and aggregating logs generated by our microservices. It uses Logstash to parse and enrich logs and Kibana for log visualization and analysis.

We use Kubernetes for container orchestration and Docker for containerization of our microservices. We have also implemented Domain-Driven Design (DDD) and Command-Query Responsibility Segregation (CQRS) patterns to achieve loose coupling between services and maintain high system availability.
