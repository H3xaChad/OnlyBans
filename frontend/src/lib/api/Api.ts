/* eslint-disable */
/* tslint:disable */
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export interface Comment {
	/** @format uuid */
	id?: string;
	content: string | null;
	/** @format uuid */
	postId: string;
	post: Post;
	/** @format uuid */
	userId: string;
	user: User;
	/** @format date-time */
	createdAt?: string;
}

export interface CommentGetDto {
	/** @format uuid */
	id?: string;
	/** @format uuid */
	postId?: string;
	/** @format uuid */
	userId?: string;
	content?: string | null;
	/** @format date-time */
	createdAt?: string;
}

/** @format int32 */
export enum ImageType {
	Value0 = 0,
	Value1 = 1,
	Value2 = 2,
	Value3 = 3,
	Value4 = 4
}

export interface LoginDto {
	/**
	 * @format email
	 * @minLength 1
	 */
	email: string;
	/** @minLength 1 */
	password: string;
}

export interface Post {
	/** @format uuid */
	id?: string;
	imageType?: ImageType;
	title?: string | null;
	description?: string | null;
	/** @format uuid */
	userId?: string;
	user?: User;
	likedByUsers?: UserPostLike[] | null;
	comments?: Comment[] | null;
}

export interface Rule {
	/** @format uuid */
	id?: string;
	text?: string | null;
	ruleCategory?: RuleEnum;
	/** @format uuid */
	userId?: string;
	user?: User;
	/** @format date-time */
	createdAt?: string;
}

export interface RuleCreateDto {
	text?: string | null;
	ruleCategory?: RuleEnum;
}

/** @format int32 */
export enum RuleEnum {
	Value0 = 0,
	Value1 = 1
}

export interface RuleGetDto {
	/** @format uuid */
	id?: string;
	text?: string | null;
	ruleCategory?: RuleEnum;
	/** @format uuid */
	userId?: string;
	/** @format date-time */
	createdAt?: string;
}

export interface User {
	/** @format uuid */
	id?: string;
	userName?: string | null;
	normalizedUserName?: string | null;
	email?: string | null;
	normalizedEmail?: string | null;
	emailConfirmed?: boolean;
	passwordHash?: string | null;
	securityStamp?: string | null;
	concurrencyStamp?: string | null;
	phoneNumber?: string | null;
	phoneNumberConfirmed?: boolean;
	twoFactorEnabled?: boolean;
	/** @format date-time */
	lockoutEnd?: string | null;
	lockoutEnabled?: boolean;
	/** @format int32 */
	accessFailedCount?: number;
	displayName?: string | null;
	state?: UserState;
	/** @format date */
	birthDate?: string;
	imageType?: ImageType;
	/** @format date-time */
	createdAt?: string;
	posts?: Post[] | null;
	comments?: Comment[] | null;
	likedPosts?: UserPostLike[] | null;
}

export interface UserCreateDto {
	/**
	 * @minLength 1
	 * @maxLength 42
	 */
	userName: string;
	/**
	 * @minLength 1
	 * @maxLength 42
	 */
	displayName: string;
	/**
	 * @minLength 1
	 * @default "user@example.com"
	 * @pattern ^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$
	 */
	email: string;
	/**
	 * @minLength 1
	 * @pattern ^\+?[1-9]\d{1,14}$
	 */
	phoneNumber: string;
	/** @format date */
	birthDate: string;
	/** @minLength 8 */
	password: string;
}

export interface UserGetDto {
	/** @format uuid */
	id?: string;
	displayName?: string | null;
	userName?: string | null;
	email?: string | null;
}

export interface UserPostLike {
	/** @format uuid */
	userId?: string;
	user?: User;
	/** @format uuid */
	postId?: string;
	post?: Post;
	/** @format date-time */
	likedAt?: string;
}

/** @format int32 */
export enum UserState {
	Value0 = 0,
	Value1 = 1,
	Value2 = 2
}

export type QueryParamsType = Record<string | number, any>;
export type ResponseFormat = keyof Omit<Body, 'body' | 'bodyUsed'>;

export interface FullRequestParams extends Omit<RequestInit, 'body'> {
	/** set parameter to `true` for call `securityWorker` for this request */
	secure?: boolean;
	/** request path */
	path: string;
	/** content type of request body */
	type?: ContentType;
	/** query params */
	query?: QueryParamsType;
	/** format of response (i.e. response.json() -> format: "json") */
	format?: ResponseFormat;
	/** request body */
	body?: unknown;
	/** base url */
	baseUrl?: string;
	/** request cancellation token */
	cancelToken?: CancelToken;
}

export type RequestParams = Omit<FullRequestParams, 'body' | 'method' | 'query' | 'path'>;

export interface ApiConfig<SecurityDataType = unknown> {
	baseUrl?: string;
	baseApiParams?: Omit<RequestParams, 'baseUrl' | 'cancelToken' | 'signal'>;
	securityWorker?: (
		securityData: SecurityDataType | null
	) => Promise<RequestParams | void> | RequestParams | void;
	customFetch?: typeof fetch;
}

export interface HttpResponse<D extends unknown, E extends unknown = unknown> extends Response {
	data: D;
	error: E;
}

type CancelToken = Symbol | string | number;

export enum ContentType {
	Json = 'application/json',
	FormData = 'multipart/form-data',
	UrlEncoded = 'application/x-www-form-urlencoded',
	Text = 'text/plain'
}

export class HttpClient<SecurityDataType = unknown> {
	public baseUrl: string = '';
	private securityData: SecurityDataType | null = null;
	private securityWorker?: ApiConfig<SecurityDataType>['securityWorker'];
	private abortControllers = new Map<CancelToken, AbortController>();
	private customFetch = (...fetchParams: Parameters<typeof fetch>) => fetch(...fetchParams);

	private baseApiParams: RequestParams = {
		credentials: 'same-origin',
		headers: {},
		redirect: 'follow',
		referrerPolicy: 'no-referrer'
	};

	constructor(apiConfig: ApiConfig<SecurityDataType> = {}) {
		Object.assign(this, apiConfig);
	}

	public setSecurityData = (data: SecurityDataType | null) => {
		this.securityData = data;
	};

	protected encodeQueryParam(key: string, value: any) {
		const encodedKey = encodeURIComponent(key);
		return `${encodedKey}=${encodeURIComponent(typeof value === 'number' ? value : `${value}`)}`;
	}

	protected addQueryParam(query: QueryParamsType, key: string) {
		return this.encodeQueryParam(key, query[key]);
	}

	protected addArrayQueryParam(query: QueryParamsType, key: string) {
		const value = query[key];
		return value.map((v: any) => this.encodeQueryParam(key, v)).join('&');
	}

	protected toQueryString(rawQuery?: QueryParamsType): string {
		const query = rawQuery || {};
		const keys = Object.keys(query).filter((key) => 'undefined' !== typeof query[key]);
		return keys
			.map((key) =>
				Array.isArray(query[key])
					? this.addArrayQueryParam(query, key)
					: this.addQueryParam(query, key)
			)
			.join('&');
	}

	protected addQueryParams(rawQuery?: QueryParamsType): string {
		const queryString = this.toQueryString(rawQuery);
		return queryString ? `?${queryString}` : '';
	}

	private contentFormatters: Record<ContentType, (input: any) => any> = {
		[ContentType.Json]: (input: any) =>
			input !== null && (typeof input === 'object' || typeof input === 'string')
				? JSON.stringify(input)
				: input,
		[ContentType.Text]: (input: any) =>
			input !== null && typeof input !== 'string' ? JSON.stringify(input) : input,
		[ContentType.FormData]: (input: any) =>
			Object.keys(input || {}).reduce((formData, key) => {
				const property = input[key];
				formData.append(
					key,
					property instanceof Blob
						? property
						: typeof property === 'object' && property !== null
							? JSON.stringify(property)
							: `${property}`
				);
				return formData;
			}, new FormData()),
		[ContentType.UrlEncoded]: (input: any) => this.toQueryString(input)
	};

	protected mergeRequestParams(params1: RequestParams, params2?: RequestParams): RequestParams {
		return {
			...this.baseApiParams,
			...params1,
			...(params2 || {}),
			headers: {
				...(this.baseApiParams.headers || {}),
				...(params1.headers || {}),
				...((params2 && params2.headers) || {})
			}
		};
	}

	protected createAbortSignal = (cancelToken: CancelToken): AbortSignal | undefined => {
		if (this.abortControllers.has(cancelToken)) {
			const abortController = this.abortControllers.get(cancelToken);
			if (abortController) {
				return abortController.signal;
			}
			return void 0;
		}

		const abortController = new AbortController();
		this.abortControllers.set(cancelToken, abortController);
		return abortController.signal;
	};

	public abortRequest = (cancelToken: CancelToken) => {
		const abortController = this.abortControllers.get(cancelToken);

		if (abortController) {
			abortController.abort();
			this.abortControllers.delete(cancelToken);
		}
	};

	public request = async <T = any, E = any>({
		body,
		secure,
		path,
		type,
		query,
		format,
		baseUrl,
		cancelToken,
		...params
	}: FullRequestParams): Promise<HttpResponse<T, E>> => {
		const secureParams =
			((typeof secure === 'boolean' ? secure : this.baseApiParams.secure) &&
				this.securityWorker &&
				(await this.securityWorker(this.securityData))) ||
			{};
		const requestParams = this.mergeRequestParams(params, secureParams);
		const queryString = query && this.toQueryString(query);
		const payloadFormatter = this.contentFormatters[type || ContentType.Json];
		const responseFormat = format || requestParams.format;

		return this.customFetch(
			`${baseUrl || this.baseUrl || ''}${path}${queryString ? `?${queryString}` : ''}`,
			{
				...requestParams,
				headers: {
					...(requestParams.headers || {}),
					...(type && type !== ContentType.FormData ? { 'Content-Type': type } : {})
				},
				signal: (cancelToken ? this.createAbortSignal(cancelToken) : requestParams.signal) || null,
				body: typeof body === 'undefined' || body === null ? null : payloadFormatter(body)
			}
		).then(async (response) => {
			const r = response.clone() as HttpResponse<T, E>;
			r.data = null as unknown as T;
			r.error = null as unknown as E;

			const data = !responseFormat
				? r
				: await response[responseFormat]()
						.then((data) => {
							if (r.ok) {
								r.data = data;
							} else {
								r.error = data;
							}
							return r;
						})
						.catch((e) => {
							r.error = e;
							return r;
						});

			if (cancelToken) {
				this.abortControllers.delete(cancelToken);
			}

			if (!response.ok) throw data;
			return data;
		});
	};
}

/**
 * @title OnlyBans
 * @version v1
 *
 * API Description for OnlyBans
 */
export class Api<SecurityDataType extends unknown> extends HttpClient<SecurityDataType> {
	auth = {
		/**
		 * No description
		 *
		 * @tags Auth
		 * @name Register
		 * @request POST:/api/v1/auth/register
		 */
		register: (data: UserCreateDto, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/auth/register`,
				method: 'POST',
				body: data,
				type: ContentType.Json,
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Auth
		 * @name Login
		 * @request POST:/api/v1/auth/login
		 */
		login: (data: LoginDto, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/auth/login`,
				method: 'POST',
				body: data,
				type: ContentType.Json,
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Auth
		 * @name LoginOauth2
		 * @request GET:/api/v1/auth/login/{providerName}
		 */
		loginOauth2: (
			providerName: string,
			query?: {
				returnUrl?: string;
			},
			params: RequestParams = {}
		) =>
			this.request<void, any>({
				path: `/api/v1/auth/login/${providerName}`,
				method: 'GET',
				query: query,
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Auth
		 * @name Logout
		 * @request POST:/api/v1/auth/logout
		 */
		logout: (params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/auth/logout`,
				method: 'POST',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Auth
		 * @name ExternalCallback
		 * @request GET:/api/v1/auth/external/callback
		 */
		externalCallback: (
			query?: {
				returnUrl?: string;
			},
			params: RequestParams = {}
		) =>
			this.request<void, any>({
				path: `/api/v1/auth/external/callback`,
				method: 'GET',
				query: query,
				...params
			})
	};
	comment = {
		/**
		 * No description
		 *
		 * @tags Comment
		 * @name GetComment
		 * @request GET:/api/v1/comment/{id}
		 */
		getComment: (id: string, params: RequestParams = {}) =>
			this.request<CommentGetDto, any>({
				path: `/api/v1/comment/${id}`,
				method: 'GET',
				format: 'json',
				...params
			})
	};
	follow = {
		/**
		 * No description
		 *
		 * @tags Follow
		 * @name Follow
		 * @request POST:/api/v1/follow/{id}
		 */
		follow: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/follow/${id}`,
				method: 'POST',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Follow
		 * @name Unfollow
		 * @request DELETE:/api/v1/follow/{id}
		 */
		unfollow: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/follow/${id}`,
				method: 'DELETE',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Follow
		 * @name GetMyFollowers
		 * @request GET:/api/v1/follow/followers
		 */
		getMyFollowers: (params: RequestParams = {}) =>
			this.request<string[], any>({
				path: `/api/v1/follow/followers`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Follow
		 * @name GetMyFollowing
		 * @request GET:/api/v1/follow/following
		 */
		getMyFollowing: (params: RequestParams = {}) =>
			this.request<string[], any>({
				path: `/api/v1/follow/following`,
				method: 'GET',
				format: 'json',
				...params
			})
	};
	post = {
		/**
		 * No description
		 *
		 * @tags Post
		 * @name GetAllPosts
		 * @request GET:/api/v1/post
		 */
		getAllPosts: (params: RequestParams = {}) =>
			this.request<UserGetDto[], any>({
				path: `/api/v1/post`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Post
		 * @name CreatePost
		 * @request POST:/api/v1/post
		 */
		createPost: (
			data: {
				/** @maxLength 42 */
				Title: string;
				/** @maxLength 1600 */
				Description: string;
				/** @format binary */
				Image: File;
			},
			params: RequestParams = {}
		) =>
			this.request<UserGetDto, any>({
				path: `/api/v1/post`,
				method: 'POST',
				body: data,
				type: ContentType.FormData,
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Post
		 * @name GetPost
		 * @request GET:/api/v1/post/{id}
		 */
		getPost: (id: string, params: RequestParams = {}) =>
			this.request<UserGetDto, any>({
				path: `/api/v1/post/${id}`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Post
		 * @name LikePost
		 * @request POST:/api/v1/post/{id}/like
		 */
		likePost: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/post/${id}/like`,
				method: 'POST',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Post
		 * @name UnlikePost
		 * @request DELETE:/api/v1/post/{id}/like
		 */
		unlikePost: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/post/${id}/like`,
				method: 'DELETE',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Post
		 * @name GetComments
		 * @request GET:/api/v1/post/{postId}/comments
		 */
		getComments: (postId: string, params: RequestParams = {}) =>
			this.request<CommentGetDto[], any>({
				path: `/api/v1/post/${postId}/comments`,
				method: 'GET',
				format: 'json',
				...params
			})
	};
	rule = {
		/**
		 * No description
		 *
		 * @tags Rule
		 * @name GetRules
		 * @request GET:/api/v1/rule
		 */
		getRules: (params: RequestParams = {}) =>
			this.request<RuleGetDto[], any>({
				path: `/api/v1/rule`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Rule
		 * @name V1RuleCreate
		 * @request POST:/api/v1/rule
		 */
		v1RuleCreate: (data: RuleCreateDto, params: RequestParams = {}) =>
			this.request<Rule, any>({
				path: `/api/v1/rule`,
				method: 'POST',
				body: data,
				type: ContentType.Json,
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Rule
		 * @name GetRule
		 * @request GET:/api/v1/rule/{id}
		 */
		getRule: (id: string, params: RequestParams = {}) =>
			this.request<RuleGetDto, any>({
				path: `/api/v1/rule/${id}`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Rule
		 * @name V1RuleDelete
		 * @request DELETE:/api/v1/rule/{id}
		 */
		v1RuleDelete: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/rule/${id}`,
				method: 'DELETE',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Rule
		 * @name V1RuleTitlerulesList
		 * @request GET:/api/v1/rule/titlerules
		 */
		v1RuleTitlerulesList: (params: RequestParams = {}) =>
			this.request<string[], any>({
				path: `/api/v1/rule/titlerules`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags Rule
		 * @name V1RuleContentrulesList
		 * @request GET:/api/v1/rule/contentrules
		 */
		v1RuleContentrulesList: (params: RequestParams = {}) =>
			this.request<string[], any>({
				path: `/api/v1/rule/contentrules`,
				method: 'GET',
				format: 'json',
				...params
			})
	};
	user = {
		/**
		 * No description
		 *
		 * @tags User
		 * @name Me
		 * @request GET:/api/v1/user/me
		 */
		me: (params: RequestParams = {}) =>
			this.request<UserGetDto, any>({
				path: `/api/v1/user/me`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags User
		 * @name GetAllUsers
		 * @request GET:/api/v1/user
		 */
		getAllUsers: (params: RequestParams = {}) =>
			this.request<UserGetDto[], any>({
				path: `/api/v1/user`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags User
		 * @name GetUser
		 * @request GET:/api/v1/user/{id}
		 */
		getUser: (id: string, params: RequestParams = {}) =>
			this.request<UserGetDto, any>({
				path: `/api/v1/user/${id}`,
				method: 'GET',
				format: 'json',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags User
		 * @name GetMyAvatar
		 * @request GET:/api/v1/user/avatar
		 */
		getMyAvatar: (params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/user/avatar`,
				method: 'GET',
				...params
			}),

		/**
		 * No description
		 *
		 * @tags User
		 * @name GetAvatar
		 * @request GET:/api/v1/user/{id}/avatar
		 */
		getAvatar: (id: string, params: RequestParams = {}) =>
			this.request<void, any>({
				path: `/api/v1/user/${id}/avatar`,
				method: 'GET',
				...params
			})
	};
}
