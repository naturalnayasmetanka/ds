import { apiClient } from "@/shared/api/axios.instance";

export const locaitonsAPI = {
  getLocations: async (): Promise<LocationDTO[]> => {
    const locations = await apiClient.get<LocationDTO[]>(
      "/Locations/locations",
    );

    return locations.data;
  },

  createLocation: (newLocation: CreateLocationDTO) => {
    const createLocation = apiClient
      .post("/Locations/locations", newLocation)
      .then((res) => res.data);

    return createLocation;
  },
};
