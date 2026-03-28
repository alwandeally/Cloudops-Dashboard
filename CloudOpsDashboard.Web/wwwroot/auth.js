window.authStorage = {
    setToken: function (token) {
        localStorage.setItem("cloudops_token", token);
    },
    getToken: function () {
        return localStorage.getItem("cloudops_token");
    },
    setUser: function (user) {
        localStorage.setItem("cloudops_user", user);
    },
    getUser: function () {
        return localStorage.getItem("cloudops_user");
    },
    setRole: function (role) {
        localStorage.setItem("cloudops_role", role);
    },
    getRole: function () {
        return localStorage.getItem("cloudops_role");
    },
    clear: function () {
        localStorage.removeItem("cloudops_token");
        localStorage.removeItem("cloudops_user");
        localStorage.removeItem("cloudops_role");
    }
};

window.downloadFileFromUrl = (url) => {
    const link = document.createElement('a');
    link.href = url;
    link.download = '';
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};