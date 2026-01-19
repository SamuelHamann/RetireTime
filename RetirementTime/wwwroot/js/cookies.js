window.RetireTimeCookies = {
    getCookie: function(name) {
        const value = `; ${document.cookie}`;
        const parts = value.split(`; ${name}=`);
        if (parts.length === 2) {
            return parts.pop().split(';').shift();
        }
        return null;
    },
    
    setCookie: function(name, value, maxAgeSeconds) {
        document.cookie = `${name}=${value}; path=/; max-age=${maxAgeSeconds}; SameSite=Strict`;
    },
    
    deleteCookie: function(name) {
        document.cookie = `${name}=; path=/; expires=Thu, 01 Jan 1970 00:00:00 GMT`;
    }
};

