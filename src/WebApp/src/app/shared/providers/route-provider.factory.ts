import { Breadcrumb } from "../models/breadcrumb.model";

export class RouteFactory {

    static createRoute(path: string, label: string, url: string, icon?: string) {

        if (!path || !label || !url) {
            throw new Error('Invalid route configuration');
        }

        const breadcrumb: Breadcrumb = {
            label: label,
            url: url,
            icon: icon
        };

        return {
            path: path,
            breadcrumb: breadcrumb
        };
    }
}