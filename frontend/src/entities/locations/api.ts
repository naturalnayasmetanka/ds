import { apiClient } from "@/shared/api/axios.instance";
import { PaginationResponse } from "@/shared/api/types";

export const locaitonsAPI = {
  getLocations: async (): Promise<LocationDTO[]> => {
    const locations = await apiClient.get<LocationDTO[]>(
      "/locations",
    );

    return locations.data;
  },

  createLocation: (newLocation: CreateLocationDTO) => {
    const createLocation = apiClient
      .post("/locations", newLocation)
      .then((res) => res.data);

    return createLocation;
  },
};


