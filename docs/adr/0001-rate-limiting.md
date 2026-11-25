# 1. Rate limit database-backed pages per client IP

Date: 2026-11-25 · Status: Accepted

## Context

The site runs on Azure App Service Free (F1) with an Azure SQL free-offer database. The database has a monthly compute allowance and auto-pauses for the rest of the month once it is used up, which takes the destination pages offline. Output caching already absorbs repeat views of the same URL, but a client can bypass the cache with arbitrary query values (`?continent=...`) and make every request hit the database.

App Service terminates TLS at a shared front end. Without forwarded headers, every request appears to come from the front end, so a per-IP limit would put all visitors in one bucket.

## Decision

- Use ASP.NET Core's built-in rate limiter: a fixed window of 60 requests per minute per client IP (`RateLimiting:PermitPerMinute`), returning **429** with `Retry-After`.
- Apply it to the pages that query the database: `DestinationsController` and `HomeController.Index`.
- Exempt static pages (About, Privacy, Gallery, Tips, status and error pages, `/healthz`, static files). They never touch the database, so limiting them protects nothing.
- Read the client IP from `X-Forwarded-For` with `UseForwardedHeaders()`, trusting the last hop only (`ForwardLimit` 1). Requests can only reach the app through the App Service front end, which appends the real client IP.
- Run the limiter after the output cache, so cached responses never count against a client.

## Consequences

- One client can make at most 60 uncached database page requests per minute, far above normal browsing and low enough to bound abuse.
- Clients behind one shared IP (a campus or carrier NAT) share the limit. At 60 per minute that is acceptable for this site.
- IPv6 clients are limited per full address, so a client rotating through its own /64 could exceed the limit. Revisit if abuse appears.
- The limit is in-memory per instance. F1 runs one instance, so it holds; a scaled-out plan would need a distributed limiter.
