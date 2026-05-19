# Interview Prep for Engineering Manager
The purpose of this project is to use claude code to setup a pair programming interview practice session in .net
## Installables/Requirements
1. Install Git and GitGUI for windows, create account, login
1. Install Claude code, get pro subscription
1. Install VS Code

## Claude Prompt

### Prompt
Create a pair programming interview that I can practice for this job in .net and lets run through it so I can practice.  I want to be the driver and you be the navigator. Here is the job description and the transcript from the first interview for context. I'd like a new git repository that I can commit to, a codebase with some interview ask that we can pair program through together, and I might need to install .net because I don't have it installed locally.  I do have vs code, git for windows installed already and I'd like to use those.  

### Context
1. Recording transcript of first interview
1. Job description

## Task

Your task as driver: Implement FindAvailableAssigneeAsync in TicketService.cs so that it returns the team member who currently has the fewest open tickets, as long as they haven't hit their MaxCapacity. Then wire it into the Create action in the controller.

This is exactly the kind of problem Mitch described — not super complex, but it reveals how you think: do you ask clarifying questions, do you reach out for input, do you consider edge cases?

So — before you write any code, what questions do you have for me about the requirements?