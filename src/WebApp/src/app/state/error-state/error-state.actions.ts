
// NGXS actions for error state
export class AddApplicationError {
    static readonly type = '[Error] Add Application Error';
    constructor(public payload: any) {}
}

export class AddNetworkError {
    static readonly type = '[Error] Add Network Error';
    constructor(public payload: any) {
        console.debug('[ErrorState] [AddNetworkError] ', payload);
    }
}

export class AddUnknownError {
    static readonly type = '[Error] Add Unknown Error';
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

export class ClearUnknownError {
    static readonly type = '[Error] Clear Unknown Error';
    constructor() {}
}
