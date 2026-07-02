# LowCortisol Platform Bounded Context Status

This document records which backend bounded contexts expose real REST contracts
and which product areas are intentionally documented without fake endpoints.

## Implemented Backend Contexts

### Workplace

Owns the physical model for sites, rooms and device groups. It exposes the
contracts used by the frontend to inspect and organize the operational layout.

### DeviceControl

Owns devices, sensors, valves, device commands, valve operations and mitigation
execution records. It integrates with Notification through application facades
instead of direct repository access.

### Monitoring

Owns readings, thresholds, anomalies and operational summaries. Readings can
produce anomalies when thresholds are exceeded.

### Notification

Owns alerts, incidents, channels and internal delivery records. It can react to
critical anomalies and coordinate incident response.

### Iam

Owns minimal identity and access behavior for sign-up, sign-in and user listing.
JWT generation is configured but authorization is not activated yet, so existing
public routes remain unchanged.

## Documented Product Contexts Without Backend Endpoints

### Plan

The frontend contains plan and access-limit behavior as part of the product
experience. The backend does not expose a Plan context yet. A future backend
implementation should include:

- plan catalog resources;
- subscription or active plan aggregate;
- commands for plan checkout or change requests;
- queries for current plan, usage and limits;
- integration with Iam for account ownership;
- integration with Workplace and DeviceControl for limit enforcement.

No fake Plan endpoints are exposed in this release.

### Support

The frontend contains support tickets, agents and help-article screens as part
of the product experience. The backend does not expose a Support context yet. A
future backend implementation should include:

- support ticket aggregate;
- ticket message entity;
- support agent or assignment model;
- knowledge article resources;
- commands for ticket creation, reply and closure;
- queries for ticket detail, ticket list and article search;
- optional integration with Notification for status changes.

No fake Support endpoints are exposed in this release.

## Rules For Future Expansion

- Do not add placeholder controllers just to make Swagger look larger.
- Keep routes public until explicit authorization activation is requested.
- Keep Resources and Assemblers in Interfaces.
- Keep business rules in Domain or Application services.
- Use cross-context facades or ACLs instead of direct repository access across
  bounded contexts.
