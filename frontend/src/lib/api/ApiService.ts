import { Api } from './Api'; // Adjust the relative path if needed

class ApiService {

    public static ONLYBANS_API_URL = 'http://localhost:5107'

    private static instance: Api<unknown>;
    
    private constructor() {}

    public static getInstance(): Api<unknown> {
        if (!ApiService.instance) {
            const API_BASE_URL = import.meta.env.ONLYBANS_API_URL || this.ONLYBANS_API_URL
            ApiService.instance = new Api<unknown>({
                baseUrl: API_BASE_URL,
                customFetch: (input: RequestInfo | URL, init?: RequestInit): Promise<Response> =>
                    fetch(input, {
                        ...init,
                        credentials: 'include'
                    })
            })
        }
        return ApiService.instance;
    }
}

export const api = ApiService.getInstance()
