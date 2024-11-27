import axios, { Axios, AxiosError } from 'axios'
const API_URL = `${import.meta.env.VITE_API_BASE_URL}`
const apiClient = axios.create({
  baseURL: API_URL, // Adjust this to match your backend URL
  headers: {
    'Content-Type': 'application/json'
  }
})

export default apiClient

export interface userInfo {
  userName:string
  email:string
  anonymousData: boolean
  name?: string
  middleName?: string
  lastName?: string
}

export const updateUserInfo = async (user: userInfo): Promise<number> => {
  const url = `${API_URL}/Auth/update-user-info`;
  const token = localStorage.getItem('token');

  // Check for missing token
  if (!token) {
    throw new Error('Authentication token is missing. Please log in again.');
  }

  try {
    // Make the API call
    const response = await axios.put(url, user, {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });

    return response.status; // Return the HTTP status code
  } catch (error) {
    // Handle Axios errors
    if (axios.isAxiosError(error) && error.response) {
      console.error('API Response Error:', error.response.data);
      throw new Error(
        error.response.data?.message || 'Failed to update user information.'
      );
    }

    // Handle other types of errors
    console.error('Unexpected Error:', error);
    throw new Error('A network or server error occurred. Please try again.');
  }
};


export const changePassword = async (
  currentPassword: string,
  newPassword: string
): Promise<number> => {
  try {
    const url = `${API_URL}/Auth/change-password`
    const token = localStorage.getItem('token')
    if (!token) {
      throw new Error('Unable to retrieve token')
    }

    // Make the API call to change the password
    const response = await axios.post(
      `${API_URL}/Auth/change-password`,
      { currentPassword, newPassword },
      { headers: { Authorization: `Bearer ${token}` } }
    )

    return response.status // Return the HTTP status code directly
  } catch (error: unknown) {
    const axiosError = error as AxiosError
    if (axiosError.response) {
      throw new Error(axiosError.message || 'Failed to change password')
    }
    throw new Error('Network or server error occurred')
  }
}

export const userInfo = async (): Promise<any> => {
  try {
    const url = `${API_URL}/auth/getInfo`
    const token = localStorage.getItem('token')
    const response = axios.get(url, { headers: { Authorization: `Bearer ${token}` } })

    // console.log('res data:', response)

    const userInfo = await response
    return userInfo
  } catch (error: unknown) {
    const axiosError = error as AxiosError
    console.error('getUserInfo error:', axiosError)

    if (axiosError.response) {
      const statusCode = axiosError.response.status
      switch (statusCode) {
        case 401:
          console.error('Unauthorized access')
          // Implement token refresh or re-authentication logic here
          break
        default:
          throw new Error(`API request failed with status code ${statusCode}`)
      }
    } else {
      throw new Error('Network or server error occurred')
    }
  }
}
