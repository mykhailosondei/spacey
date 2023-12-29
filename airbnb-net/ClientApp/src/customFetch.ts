import jwtDecode from 'jwt-decode';

const customFetch = async (url: string, options: RequestInit = {}) => {
  let storageToken : string | null = localStorage.getItem('token');
  if (storageToken) {
    const decodedToken : any = jwtDecode.jwtDecode(storageToken);
    if (decodedToken.exp * 1000 < Date.now()) {
      localStorage.removeItem('token');
      storageToken = null;
    }
    options.headers = {
        ...options.headers,
        Authorization: `Bearer ${storageToken}`
    };
  }

  return await fetch(url, options);
}

export default customFetch;