export type Contact = {
  id: number;
  name: string;
  email: string;
  phone?: string;
  note?: string;
};

export type ContactCreateDto = {
  name: string;
  email: string;
  phone?: string;
  note?: string;
};

// Additional types can be added here as needed