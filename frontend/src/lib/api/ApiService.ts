import { Api } from './Api'; // Adjust the relative path if needed

class ApiService {

    private static instance: Api<unknown>;
    
    private constructor() {}

    public static getInstance(): Api<unknown> {
        if (!ApiService.instance) {
            const API_BASE_URL: string = import.meta.env.VITE_API_URL || 'http://localhost:5107';
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

export const api = ApiService.getInstance();
