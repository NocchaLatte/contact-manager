import axios from 'axios';

const baseURL = process.env.REACT_APP_API_BASE_URL || 'http://localhost:5217';

export const api = axios.create({
  baseURL,
  headers: { 'Content-Type': 'application/json' },
});

// helper function to handle global error extraction
export function extractProblemMessage(e: any): string {
  return e?.response?.data?.detail
      || e?.response?.data?.title
      || e?.message
      || 'Unknown error';
}
