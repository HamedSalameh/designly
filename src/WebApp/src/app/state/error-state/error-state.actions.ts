
// NGXS actions for error state
export class AddApplicationError {
    static readonly type = '[Error] Add Application Error';
    constructor(public payload: any) {}
}

export class AddNetworkError {
    static readonly type = '[Error] Add Network Error';
    constructor(public payload: any) {}
}

export class AddServerError {
    static readonly type = '[Error] Add Server Error';
    constructor(public payload: any) {}
}

export class ClearApplicationError {
    static readonly type = '[Error] Clear Application Error';
    constructor() {}
}

export class ClearNetworkError {
    static readonly type = '[Error] Clear Network Error';
    constructor() {}
}

export class ClearServerError {
    static readonly type = '[Error] Clear Server Error';
    constructor() {}
}
