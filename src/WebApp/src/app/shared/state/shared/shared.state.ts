export interface SharedState {
    loading: boolean;
    activeModule: string;
}

export const InitialSharedState: SharedState = {
    loading: false,
    activeModule: ''
};

