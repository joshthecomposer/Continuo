# Continuo  
## Music Practice Journal App  
### Minimum Viable Product
:white_square_button: Users can create and schedule practice sessions with tasks.  
:white_check_mark: App features a dynamically rendered calendar for scheduling.  
:white_square_button: Practicce sessions can be repeated automatically or as often as wanted on a dynamic calendar.  
:white_check_mark: Tasks can be shared between sessions and vice-versa (Many-to-Many)  
:white_check_mark: Tasks are associated with instruments on a one-to-one basis, but Sessions can contain tasks of different instruments. (User might practice a scale on a guitar and an Etude on Piano in one practice session).  
:white_square_button: App provides a metronome for use during practice.  
:white_check_mark: App shares a common API that is called cross-platform.  
:white_square_button: Client will be migrated to Blazor hybrid and output to IOS, Android and Web  
:white_square_button: Practice session state is cached in the DB and shared across platforms.  
:white_check_mark: API features a short term JWT Auth system (One minute expiry) with a long term Refresh Token stored on the Database.
### Product backlog
* Windows and Mac Desktop build.
* Video recording capability for posture and technique practice session review.
* App provides a tuner.
* Access keys can be generated for teachers to track student's practice progress.
