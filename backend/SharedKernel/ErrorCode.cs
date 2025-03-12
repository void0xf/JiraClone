namespace SharedKernel;

    public enum ErrorCode
    {
        // General errors (1000-1099)
        UnknownError = 1000,
        ValidationFailed = 1001,
        NotFound = 1002,
        Unauthorized = 1003,
        Forbidden = 1004,
        Conflict = 1005,
        
        // User domain errors (2000-2099)
        UserNotFound = 2000,
        UserAlreadyExists = 2001,
        InvalidCredentials = 2002,
        
        // Project domain errors (3000-3099)
        ProjectsFailedToLoad = 3000,
    }

