# Docker Notes

### Helpful hints/behaviors for docker.

1. For port binding, the left value is the host port and the right value is the
container port. The container port can be anything, so long as there are no
conflicting host ports (make use of docker networks).
2. `ARG` before the first `FROM` applies only to that `FROM`. `FROM` resets all
arguments after it's called.
3. `ENTRYPOINT` nor `CMD` expand `ARGS`. You need to use an environment variable
(`ENV`) to pass in arguments to the command.
