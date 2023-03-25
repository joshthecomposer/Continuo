# Continuo  
## Music Practice Journal App  
### Minimum Viable Product
* Users can create and schedule practice sessions with tasks.
* Sessions can be repeated automatically or as often as wanted on a dynamic calendar.
* Tasks can be shared between sessions and vice-versa (Many-to-Many)
* Tasks are associated with instruments on a one-to-one basis, but Sessions can contain tasks of different instruments. (User might practice a scale on a guitar and an Etude on Piano in one practice session).
* App provides a metronome for use during practice.  
* App shares a common API that is called cross-platform (Client will be developed in blazor hybrid and output to IOS, Android and Web)
* Practice session state is cached in the DB and shared across platforms.
:white_check_mark:* API features a short term JWT Auth system (One minute expiry) with a long term Refresh Token stored on the Database.
### Product backlog
* Windows and Mac Desktop build.
* Video recording capability for posture and technique practice session review.
* App provides a tuner.
* Access keys can be generated for teachers to track student's practice progress.