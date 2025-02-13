export interface PostCreateDto {
	/**
	 * @maxLength 42
	 */
	title: string
	/**
	 * @maxLength 1600
	 */
	description: string
    /**
	 * File upload for image
	 */
    image: File
}