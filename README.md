# Reporting service

Imagine that you are asked to fix a reporting service that was done by a developer who quit before you.
All you know about the service is that it's supposed to produce an accounting report for a selected month
for all employees in the company. There is no other functionality in the reporting service.

**The report looks like this:**

January 2017

---
FinDepartment

Andrey Sergeyevich Bubnov 70000r

Grigory Evseevich Zinoviev 65000p

Yakov Mikhailovich Sverdlov 80000r

Alexey Ivanovich Rykov 90000r

Total for the department 305000r

---
Accounting

Vasily Vasilyevich Kuznetsov 50000r

Demyan Sergeyevich Korotchenko 55000r

Mikhail Andreyevich Suslov 35000r

Total for the department 140000r

---  
IT

Frol Romanovich Kozlov 90000r

Dmitry Stepanovich Polyanski 120000r

Andrei Pavlovich Kirilenko 110000r

Arvid Janovich Pelshe 120000r

Total for the department 440000r

--- 

Total for the company 885000r




The report service has already been launched in production, but it works very unstable. Since some time it
stopped working at all. Your task is to fix the reporting service. Everything that is known about the inner workings
of the service is described in a memo from the previous developer:

> The list of employees by department can be taken from the employee database. Salary of an employee per month can be obtained in the web-service of the accounting department, but it needs to receive the employee code from the HR service as input

There is also a list of bugs from users (from those times when the reporting service worked somehow):

+ sometimes the system does not give the report, but returns an error
+ works very slowly
+ not all employees are included in the report
+ there is no line for the whole company

You need to bring the reporting service back to life, fix known bugs and put the project in order, because the previous developer was not very careful.
You do not have access to the database and web services, because the information in them is strictly confidential. Yes, we've heard something about tests and it would be nice to cover the code with a test so that we don't get into such a situation again.
