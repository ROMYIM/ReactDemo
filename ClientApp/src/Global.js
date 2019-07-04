export const globalValue = new Global();

function Global() {
    
    this._token = undefined;
    
}

Object.defineProperty(globalValue, "Token", {

    configurable: false,

    get: function () {
        return this._token;
    },

    set: function (params) {
        if (params !== this._token && params) {
            this._token = params;
        }
    }
})
